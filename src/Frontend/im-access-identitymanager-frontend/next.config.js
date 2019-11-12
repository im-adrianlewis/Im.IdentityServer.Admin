const { BundleAnalyzerPlugin } = require('webpack-bundle-analyzer');
const withSass = require('@zeit/next-sass');
const withGraphql = require('next-plugin-graphql');
const withOffline = require('next-offline');

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
