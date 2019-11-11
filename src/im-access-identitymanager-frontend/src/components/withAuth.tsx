import * as React from 'react';
import {  } from 'next';

interface WithAuthProps {
  isAuthenticated: boolean;
}

const withAuth = <P extends object>(Component: React.ComponentType<P>) =>
  class WithAuth extends React.Component<P & WithAuthProps> {
    static async getInitialProps({req: Request}) {
      if (req.user) {
        
      }
    }

    render() {

    }
  }
}