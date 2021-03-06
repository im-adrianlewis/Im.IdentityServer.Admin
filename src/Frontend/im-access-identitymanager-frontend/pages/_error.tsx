import * as React from 'react';
import Link from 'next/link';
import { withRouter } from 'next/router';
import { WithRouterProps } from 'next/dist/client/with-router';
import { Container } from 'reactstrap';
import { NextPageContext } from 'next';

interface ErrorPageProps extends WithRouterProps {
  errorCode: number;
}

class ErrorPage extends React.Component<ErrorPageProps> {
  static getInitialProps(context: NextPageContext) {
    const errorCode = 
      context.res 
      ? context.res.statusCode 
      : (context.err ? context.err.statusCode : null);
      
    return {
      errorCode
    };
  }

  render() {
    var response;
    switch (this.props.errorCode) {
      case 200: // Also display a 404 if someone requests /_error explicitly
      case 404:
        response = (
          <div>
            <Container className="pt-5 text-center">
              <h1 className="display-4">Page Not Found</h1>
              <p>The page <strong>{ this.props.router.pathname }</strong> does not exist.</p>
              <p><Link href="/"><a>Home</a></Link></p>
            </Container>
          </div>
        );
        break;
      case 500:
        response = (
          <div>
            <Container className="pt-5 text-center">
              <h1 className="display-4">Internal Server Error</h1>
              <p>An internal server error occurred.</p>
            </Container>
          </div>
        );
        break;
      default:
        response = (
          <div>
            <Container className="pt-5 text-center">
              <h1 className="display-4">HTTP { this.props.errorCode } Error</h1>
              <p>
                An <strong>HTTP { this.props.errorCode }</strong> error occurred while
                trying to access <strong>{ this.props.router.pathname }</strong>
              </p>
            </Container>
          </div>
        );
        break;
    }

    return response;
  }
}

export default withRouter(ErrorPage);
