const { BundleAnalyzerPlugin } = require('webpack-bundle-analyzer');
const withSass = require('@zeit/next-sass');
const withGraphql = require('next-plugin-graphql');
const withOffline = require('next-offline');

const frontEndEnvKeys = [
  'IDENTITY_URL',
  'IDENTITY_CLIENT_ID',
  'GRAPHQI_REGULAR_URI',
  'GRAPHQI_SUBSCRIPTIONS_URI'
];

const envPlugin = frontEndEnvKeys.reduce(
  (result, key) => 
    Object.assign({}, result, {
      [`process.env.${key}`]: JSON.stringify(process.env[key])
    }),
  {}
);

module.exports = withGraphql(withOffline(withSass({
  webpack(config, { buildId, dev, isServer, defaultLoaders, webpack }) {
    const csTarget = isServer ? 'server-side' : 'client-side';
    const environment = dev ? 'DEV environment' : 'PROD environment';
    console.log(`Running ${csTarget} webpack ${environment} build for build:${buildId}...`);

    if (process.env.ANALYZE) {
      config.plugins.push(new BundleAnalyzerPlugin({
        analyzerMode: 'server',
        analyzerPort: 8888,
        openAnalyzer: true
      }));
    }

    config.node = { fs: 'empty' };
    config.plugins.push(new webpack.DefinePlugin(envPlugin));
    return config;
  },
  // webpackDevMiddleware(config) {
  //   // TODO: Setup/override dev middleware configuration
  //   return config;
  // },
  cssModules: true,
  cssLoaderOptions: { },
  workerLoaderOptions: { },
  crossOrigin: 'anonymous',
  generateBuildId: async () => {
    if (process.env.BUILD_ID) {
      return process.env.BUILD_ID;
    }

    // Fallback to default build id generation
    return null;
  }
})));
