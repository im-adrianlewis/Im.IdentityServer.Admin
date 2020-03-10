import * as React from 'react';
import { NavItem, NavLink } from 'reactstrap';
import Link from 'next/link';

export class AccountNav extends React.Component {

  public render() {
    return this.isLoggedIn ? this.renderForAuthenticated() : this.renderForUnauthenticated();
  }

  private isLoggedIn() {
    return true;
  }
  
  private renderForUnauthenticated() {
    return (
      <NavItem>
        <Link href="/auth/signin">
          <NavLink href="/auth/signin">Sign In</NavLink>
        </Link>
      </NavItem>
    );
  }

  private renderForAuthenticated() {
    return (
      <NavItem className="dropdown no-arrow">
        <a className="nav-link dropdown-toggle" href="#" id="accountDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
          <span className="mr-2 d-none d-lg-inline text-gray-600">Logged in user</span>
        </a>
        <div className="dropdown-menu dropdown-menu-right shadow animated--grow-in" aria-labelledby="accountDropdown">
          <a className="dropdown-item" href="#">Profile</a>
          <div className="dropdown-divider"></div>
          <a className="dropdown-item" href="#">Sign Out</a>
        </div>
      </NavItem>
    );
  }
}
