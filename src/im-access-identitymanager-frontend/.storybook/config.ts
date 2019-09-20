import { configure, addParameters } from '@storybook/react';
import { DocsPage, DocsContainer } from '@storybook/addon-docs/blocks';
import { themes } from '@storybook/theming';

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
