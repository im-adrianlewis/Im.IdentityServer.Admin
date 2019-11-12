const path = require('path');

module.exports = ({ config, mode }) => {
  config.module.rules.push(
    {
      test: /\.(ts|tsx)$/,
      use: [
        {
          loader: require.resolve('babel-loader'),
          options: {
            presets: [['react-app', { flow: false, typescript: true }]],
          }
        },
        {
          loader: require.resolve('react-docgen-typescript-loader')
        }
      ],
    },
    {
      test: /\.(scss|sass)$/,
      use: [
        {
          loader: require.resolve('style-loader')
        },
        {
          loader: require.resolve('css-loader')
        },
        {
          loader: require.resolve('sass-loader')
        }
      ]
    });

  config.resolve.extensions.push('.ts', '.tsx');
  return config;
};
