const withSass = require('@zeit/next-sass');
const {BundleAnalyzerPlugin} = require('webpack-bundle-analyzer');

module.exports = withSass({
  webpack(config, options) {
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
  cssModules: true,
  crossOrigin: 'anonymous',
  generateBuildId: async () => {
    if (process.env.BUILD_ID) {
      return process.env.BUILD_ID;
    }

    // Fallback to default build id generation
    return null;
  },
  serverRuntimeConfig: {
    sslCertificateFilename: process.env.SSL_CERT_PFXFILE,
    sslCertificatePassphrase: process.env.SSL_CERT_PASSPHRASE
  },
  publicRuntimeConfig: {

  }
});
