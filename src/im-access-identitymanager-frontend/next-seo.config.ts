import { SERVER_URL } from './src/constants/env';

export default {
  titleTemplate: `%s | Identity Manager`,
  openGraph: {
    type: 'website',
    locale: 'en-GB',
    url: `${SERVER_URL}`,
    site_name: 'IM Access Identity Manager'
  }
};
