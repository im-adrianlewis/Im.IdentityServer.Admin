import * as React from 'react';
import { Nav, NavItem, NavLink } from 'reactstrap';
import Link from 'next/link';
import css from '../../css/index.scss';

export class AccountNav extends React.Component {
  
  public render() {
    return (
      <Nav className="ml-auto" navbar cssModule={css}>
        <NavItem cssModule={css}>
          <Link href="/auth/signin">
            <NavLink cssModule={css} href="/auth/signin">Sign In</NavLink>
          </Link>
        </NavItem>
      </Nav>
    );
  }
}
