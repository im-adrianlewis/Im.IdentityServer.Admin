import moment from 'moment';
import { Client, TokenSet, UserinfoResponse } from 'openid-client';

export class UserIdentity {
  constructor(tokenSet: TokenSet) {
    this.accessToken = tokenSet.access_token ?? '';
    this.refreshToken = tokenSet.refresh_token ?? '';
    this.idToken = tokenSet.id_token ?? '';
       
    const now = moment.utc();
    var expiresIn = tokenSet.expires_in ?? 3600;
    this.refreshAfter = now.add(expiresIn / 2, 's');
    this.expiresAfter = now.add(expiresIn, 's');
  }

  readonly accessToken: string;
  readonly refreshToken: string;
  readonly idToken: string;
  readonly refreshAfter: moment.Moment;
  readonly expiresAfter: moment.Moment;
}

export class User {
  constructor(client: Client, tokenSet: TokenSet, userInfo: UserinfoResponse) {
    this._client = client;

    this.id = userInfo.sub;
    this.displayName = userInfo.name ?? userInfo.preferred_username ?? '';
    this.email = userInfo.email ?? '';
    this.emailVerified = userInfo.email_verified ?? false;
    this.userInfo = userInfo;
    
    this._tokenSet = tokenSet;
    this._identity = new UserIdentity(tokenSet);
  }
 
  readonly id: string;
  readonly displayName: string;
  readonly email: string;
  readonly emailVerified: boolean;
  readonly userInfo: UserinfoResponse;

  get identity(): UserIdentity {
    return this._identity;
  }

  get tokenSet(): TokenSet {
    return this._tokenSet;
  }

  async getAccessToken(): Promise<string> {
    await this.refreshAccessTokenIfNeeded();
    return this._identity.accessToken;
  }

  async refreshAccessTokenIfNeeded(): Promise<void> {
    if (this._identity.refreshToken !== '' && this._identity.refreshAfter < moment.utc()) {
      var tokenSet = await this._client.refresh(this._identity.refreshToken);

      this._tokenSet = tokenSet;
      this._identity = new UserIdentity(tokenSet);
    }
  }

  private _client: Client;
  private _identity: UserIdentity;
  private _tokenSet: TokenSet;
}

export class UserWriter {
  public static writeTo(user: User): string {
    const userData = new Buffer(JSON.stringify({
      tokenSet: user.tokenSet,
      userInfo: user.userInfo
    })).toString('base64');

    return userData;
  }
}

export class UserReader {
  public static readFrom(client: Client, payload: string): User {
    var userData = JSON.parse(new Buffer(payload, 'base64').toString());
    return new User(client, userData.tokenSet, userData.userInfo);
  }
}
