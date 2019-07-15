import * as React from 'react';
import { storiesOf } from '@storybook/react';
import { Header } from '../src/components/Header';
import { Footer } from '../src/components/Footer';

const stories = storiesOf('Page Components', module);

stories
  .add(
    'Header',
    () => <Header />
  )
  .add(
    'Footer',
    () => <Footer />
  );
