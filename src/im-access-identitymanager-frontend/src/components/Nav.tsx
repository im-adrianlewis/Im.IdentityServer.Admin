import * as React from 'react';
import { Navbar, NavbarBrand, NavbarToggler, Nav, NavItem, NavLink, Collapse } from 'reactstrap';
import Link from 'next/link';
import css from '../../css/index.scss';

interface MainMenuProps {

}

interface MainMenuState {
  isOpen: boolean;
}

export class MainMenu extends React.Component<MainMenuProps, MainMenuState> {
  constructor(props: MainMenuProps) {
    super(props);
    
    this.toggle = this.toggle.bind(this);
    this.state = {
      isOpen: false
    };
  }

  public toggle() {
    this.setState({
      isOpen: !this.state.isOpen
    });
  }
  
  public render() {
    return (
      <Navbar color="light" light expand="md" cssModule={css}>
        <NavbarToggler id="navbar-toggler" onClick={this.toggle} cssModule={css} />
        <NavbarBrand href="/" cssModule={css}>Identity Manager</NavbarBrand>
        <Collapse isOpen={this.state.isOpen} navbar cssModule={css}>
          <Nav className="ml-auto" navbar cssModule={css}>
            <NavItem cssModule={css}>
              <Link href="/">
                <NavLink href="/">Home</NavLink>
              </Link>
            </NavItem>
            <NavItem cssModule={css}>
              <Link href="/SSR">
                <NavLink href="/SSR">SSR</NavLink>
              </Link>
            </NavItem>
            <NavItem cssModule={css}>
              <Link href="/StyledJsx">
                <NavLink href="/StyledJsx">Styled Jsx</NavLink>
              </Link>
            </NavItem>
            <NavItem cssModule={css}>
              <Link href="/ModuleCss">
                <NavLink href="/ModuleCss">Module CSS</NavLink>
              </Link>
            </NavItem>
          </Nav>
        </Collapse>
      </Navbar>
    );
  }
}
