const dotenvPlugin = require('dotenv-webpack');
const withSass = require('@zeit/next-sass');
//const withCss = require('@zeit/next-css');

const {BundleAnalyzerPlugin} = require('webpack-bundle-analyzer');

module.exports = withSass({
  // webpack(config, options) {
  //   //config.node = { fs: require('fs-extra') };
  //   config.resolve.alias.fs = 'fs-extra';
  //   return config;
  // },
  webpack(config, { buildId, dev, isServer, defaultLoaders, webpack }) {
    const csTarget = isServer ? 'server-side' : 'client-side';
    const environment = dev ? 'DEV environment' : 'PROD environment';
    console.log(`Running ${csTarget} webpack ${environment} build for build:${buildId}...`);

    // config.plugins.push(new webpack.ProgressPlugin(
    //   (percentage, message, ...args) => {
    //     console.info(percentage, message, ...args);
    //   }));

    if (process.env.ANALYZE) {
      config.plugins.push(new BundleAnalyzerPlugin({
        analyzerMode: 'server',
        analyzerPort: 8888,
        openAnalyzer: true
      }));
    }

    // config.plugins.push(new dotenvPlugin());

    //config.resolve.alias.fs = '/node_modules/fs-extra/lib/index.js';
    config.node = { fs: 'empty' };
    return config;
  },
  // webpackDevMiddleware(config) {
  //   // TODO: Setup/override dev middleware configuration

  //   return config;
  // },
  cssModules: true,
  // cssLoaderOptions: {
  //   importLoaders: 1
  // },
  crossOrigin: 'anonymous',
  generateBuildId: async () => {
    if (process.env.BUILD_ID) {
      return process.env.BUILD_ID;
    }

    // Fallback to default build id generation
    return null;
  },
  // serverRuntimeConfig: {
  //   sslCertificateFilename: process.env.SSL_CERT_PFXFILE,
  //   sslCertificatePassphrase: process.env.SSL_CERT_PASSPHRASE
  // },
  // publicRuntimeConfig: {

  // }
});
