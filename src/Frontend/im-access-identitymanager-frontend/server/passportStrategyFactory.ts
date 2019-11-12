import passport from 'passport';
import * as oidc from 'openid-client';

export default class PassportStrategyFactory {
  constructor(serverUrl: string) {
    this.serverUrl = serverUrl;
  }

  public getStrategy(
    tenant: string,
    client: oidc.Client,
    callback: (tokenSet: oidc.TokenSet, userInfo: oidc.UserinfoResponse, done: (err: any, user?: any) => void) => void): passport.Strategy {
    
    var strategy = this.knownStrategies[tenant];

    if (!strategy) {
      strategy = new oidc.Strategy<any, oidc.Client>({
        client: client,
        params: {
          client_id: 'ImAccessGraph',
          redirect_uri: `${this.serverUrl}/auth/signin/callback-${tenant.toLowerCase()}`,
          response_type: 'code id_token token',
          response_mode: 'form_post',
          acr_values: `tenant:${tenant}`,
          scope: 'openid profile email offline_access',
          prompt: 'login'
        },
        passReqToCallback: false,
        sessionKey: 'idtymgr_session',
        usePKCE: false
      },
      callback);

      this.knownStrategies[tenant] = strategy;
    }

    return strategy;
  }

  private knownStrategies: StringDictionary<oidc.Strategy<any, oidc.Client>> = {};
  private serverUrl: string;
}

interface StringDictionary<V> {
  [key: string]: V;
}
