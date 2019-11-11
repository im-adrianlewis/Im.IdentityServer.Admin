import * as React from 'react';
import { NavItem, NavLink } from 'reactstrap';
import Link from 'next/link';
import css from '../../css/index.scss';

export class MainNav extends React.Component {

  public render() {
    return (
      <>
        <NavItem cssModule={css}>
          <Link href="/">
            <NavLink cssModule={css} href="/">Home</NavLink>
          </Link>
        </NavItem>
        <NavItem cssModule={css}>
          <Link href="/SSR">
            <NavLink cssModule={css} href="/SSR">SSR</NavLink>
          </Link>
        </NavItem>
        <NavItem cssModule={css}>
          <Link href="/StyledJsx">
            <NavLink cssModule={css} href="/StyledJsx">Styled Jsx</NavLink>
          </Link>
        </NavItem>
        <NavItem cssModule={css}>
          <Link href="/ModuleCss">
            <NavLink cssModule={css} href="/ModuleCss">Module CSS</NavLink>
          </Link>
        </NavItem>
      </>
    );
  }
}
