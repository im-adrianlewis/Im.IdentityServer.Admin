import * as React from 'react';
import * as css from '../css/index.scss';
import { Container, Jumbotron } from 'reactstrap';
import { Home } from '../src/components/Home';
import { Layout } from '../src/components/Layout';
import { Page } from '../src/components/Page';
import classNames from 'classnames';

// import NextSeo from 'next-seo';

export default class extends Page {
  public componentDidMount() {
    if ('serviceWorker' in navigator) {
      navigator.serviceWorker
        .register('/sw.js')
        .then(registration => {
          console.log('Service worker registration successful with scope: ', registration.scope);
        })
        .catch(err => {
          console.error('Service worker registration failed', err);
        });
    } else {
      console.log('Service worker not supported');
    }
  }

  public render() {
    return (
      <Layout {...this.props} showNavMenu={true}>
        {/* <NextSeo config={{
          title: 'Home',
          description: 'Identity Manager'
        }} /> */}
        <Jumbotron
          cssModule={css}
          className="text-light rounded-0" style={{
            backgroundColor: 'rgba(73,155,234,1)',
            background: 'radial-gradient(ellipse at center, rgba(73,155,234,1) 0%, rgba(32,124,229,1) 100%)',
            boxShadow: 'inset 0 0 100px rgba(0,0,0,0.1)'
          }}>
          <Container cssModule={css} className="mt-2 mb-2">
            <h1 className={classNames('display-2', css['mb-3'])} style={{fontWeight: 300}}>
              <span style={{fontWeight: 600}}>
                <span className={css['mr-3']}>▲</span>
                <br className={classNames(css['v-block'], css['d-sm-none'])} />
                Identity Manager
              </span>
              <br className={classNames(css['v-block'], css['d-lg-none'])} />
              &nbsp;User management portal
            </h1>
            <p className={classNames('lead', css['mb-5'])}>
              A reference implementation of next.js/react/typescript/graphql/openidconnect
            </p>
            <p className={css['text-right']}>
              <a href="https://immediate.github.com/PlatformTechnology/Im.Access.Graph"
                 className={classNames(css['btn'], css['btn-outline-light'], css['btn-lg'])}>
                <span className={classNames(css['icon'], css['ion-logo-github'], css['mr-2'])} /> Download from GitHub</a>
            </p>
            <style jsx>{`
              .display-2  {
                text-shadow: 0 5px 10px rgba(0,0,0,0.3);
                color: rgba(255,255,255,0.9);
              }
              .lead {
                font-size: 3em;
                opacity: 0.7;
              }
              @media (max-width: 767px) {
                .display-2 {
                  font-size: 3em;
                  margin-bottom: 1em;
                }
                .lead {
                  font-size: 1.5em;
                }
              }
            `}</style>
          </Container>
        </Jumbotron>
        <Container>
          <Home/>
        </Container>
      </Layout>
    );
  }
}
