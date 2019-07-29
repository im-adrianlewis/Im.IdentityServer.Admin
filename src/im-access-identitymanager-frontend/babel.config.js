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
        // "react-css-modules",
        // {
        
        // }
      ]
    ]
  };
};
