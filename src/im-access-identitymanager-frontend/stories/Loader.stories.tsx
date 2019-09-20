import * as React from 'react';
import { withKnobs, boolean } from '@storybook/addon-knobs';
import Loader from '../src/components/Loader';

export default {
  title: 'Layout/Loader',
  decorators: [
    withKnobs
  ],
  parameters: {
    notes: 'Used to show the loader spinner animation either compressed or full-screen.'
  }
};

export const standard = () => <Loader fullscreen={boolean('Full-screen', false)} />;
export const compact = () => <Loader fullscreen={false} />;
export const fullscreen = () => <Loader fullscreen={true} />;
