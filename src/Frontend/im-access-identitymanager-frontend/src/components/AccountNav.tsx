import * as React from 'react';
import { NavItem, NavLink } from 'reactstrap';
import Link from 'next/link';
import css from '../../css/index.scss';
import classNames from 'classnames';

export class AccountNav extends React.Component {

  public render() {
    return this.isLoggedIn ? this.renderForAuthenticated() : this.renderForUnauthenticated();
  }

  private isLoggedIn() {
    return true;
  }
  
  private renderForUnauthenticated() {
    return (
      <NavItem cssModule={css}>
        <Link href="/auth/signin">
          <NavLink cssModule={css} href="/auth/signin">Sign In</NavLink>
        </Link>
      </NavItem>
    );
  }

  private renderForAuthenticated() {
    return (
      <NavItem cssModule={css} className={classNames(css['dropdown'], css['no-arrow'])}>
        <a className={classNames(css['nav-link'], css['dropdown-toggle'])} href="#" id="accountDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
          <span className={classNames(css['mr-2'], css['d-none'], css['d-lg-inline'], css['text-gray-600'])}>Logged in user</span>
        </a>
        <div className={classNames(css['dropdown-menu'], css['dropdown-menu-right'], css['shadow'], css['animated--grow-in'])} aria-labelledby="accountDropdown">
          <a className={css['dropdown-item']} href="#">Profile</a>
          <div className={css['dropdown-divider']}></div>
          <a className={css['dropdown-item']} href="#">Sign Out</a>
        </div>
      </NavItem>
    );
  }
}
