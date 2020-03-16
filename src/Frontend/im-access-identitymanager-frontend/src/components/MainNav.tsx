import * as React from 'react';
import { NavItem, NavLink } from 'reactstrap';
import Link from 'next/link';

export class MainNav extends React.Component {

  public render() {
    return (
      <>
        <NavItem>
          <Link href="/">
            <NavLink href="/">Home</NavLink>
          </Link>
        </NavItem>
        <NavItem>
          <Link href="/CircuitBreakerPolicies">
            <NavLink href="/CircuitBreakerPolicies">Circuit Breaker Policies</NavLink>
          </Link>
        </NavItem>
        <NavItem>
          <Link href="/SSR">
            <NavLink href="/SSR">SSR</NavLink>
          </Link>
        </NavItem>
        <NavItem>
          <Link href="/StyledJsx">
            <NavLink href="/StyledJsx">Styled Jsx</NavLink>
          </Link>
        </NavItem>
        <NavItem>
          <Link href="/ModuleCss">
            <NavLink href="/ModuleCss">Module CSS</NavLink>
          </Link>
        </NavItem>
      </>
    );
  }
}
