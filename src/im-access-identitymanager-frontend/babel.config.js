module.exports = (api) => {
  api.cache(true);

  return {
    presets: [
      'next/babel'
    ],
    plugins: [
      [
        'transform-define',
        {
          'process.env.NODE_ENV': process.env.NODE_ENV
        }
      ],
      [
        'babel-plugin-graphql-tag',
        {
          'debug': true
        }
        // "react-css-modules",
        // {
        
        // }
      ],
      [
        '@babel/plugin-proposal-nullish-coalescing-operator'
      ]
    ]
  };
};
