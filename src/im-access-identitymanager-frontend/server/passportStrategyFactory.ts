import passport from 'passport';
import { Strategy, Client, TokenSet, VerifyCallback } from 'openid-client';

export default class PassportStrategyFactory {
  constructor(serverUrl: string) {
    this.serverUrl = serverUrl;
  }

  public getStrategy(
    tenant: string,
    client: Client,
    callback: (userInfo: any, tokenSet: TokenSet, verified: VerifyCallback) => void): passport.Strategy {
    
    var strategy = this.knownStrategies[tenant];

    if (!strategy) {
      strategy = new Strategy({
        client: client,
        params: {
          client_id: 'ImAccessGraph',
          redirect_uri: `${this.serverUrl}/auth/signin/callback-${tenant}`,
          response_type: 'code id_token token',
          response_mode: 'form_post',
          acr_values: `tenant:${tenant}`,
          scope: 'openid profile',
          prompt: 'login'
        },
        passReqToCallback: false,
        sessionKey: 'oidc:identity',
        usePKCE: false
      },
      callback);

      this.knownStrategies[tenant] = strategy;
    }

    return strategy;
  }

  private knownStrategies: StringDictionary<Strategy> = {};
  private serverUrl: string;
}

interface StringDictionary<V> {
  [key: string]: V;
}
