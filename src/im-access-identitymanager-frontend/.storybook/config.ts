import { configure, addDecorator, addParameters } from '@storybook/react';
import { DocsPage, DocsContainer } from '@storybook/addon-docs/blocks';
import { withInfo } from '@storybook/addon-info';
import { themes } from '@storybook/theming';

// Setup extended information UI styling
addDecorator(
  withInfo({
    styles: {
      header: {
        h1: {
          marginRight: '20px',
          fontSize: '25px',
          display: 'inline',
        },
        body: {
          paddingTop: 0,
          paddingBottom: 0,
        },
        h2: {
          display: 'inline',
          color: '#999',
        },
      },
      infoBody: {
        backgroundColor: '#eee',
        padding: '0px 5px',
        lineHeight: '2',
      }
    },
    inline: true,
    source: false
  })
);


addParameters({
  docs: {
    container: DocsContainer,
    page: DocsPage,
  },
});

addParameters({
  darkMode: {
    // Override the default dark theme
    dark: { ...themes.dark, appBg: 'black' },
    // Override the default light theme
    light: { ...themes.normal, appBg: 'white' }
  }
});

const req = require.context('../stories', true, /.stories.(ts|tsx|mdx)$/);

function loadStories() {
  const allExports = [];
  req.keys().forEach(fname => allExports.push(req(fname)));
  return allExports;
}

configure(loadStories, module);
