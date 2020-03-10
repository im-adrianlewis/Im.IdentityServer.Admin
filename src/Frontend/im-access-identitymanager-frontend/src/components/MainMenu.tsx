import * as React from 'react';
import { Nav, Navbar, NavbarBrand, NavbarToggler, Collapse } from 'reactstrap';
import { AccountNav } from './AccountNav';
import { MainNav } from './MainNav';

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
      <Navbar color="light" light expand="md">
        <NavbarToggler id="navbar-toggler" onClick={this.toggle} />
        <NavbarBrand href="/">Identity Manager</NavbarBrand>
        <Collapse isOpen={this.state.isOpen} navbar>
          <Nav className="ml-auto" navbar>
            <MainNav />
            <div className="topbar-divider d-none d-sm-block"></div>
            <AccountNav />
          </Nav>
        </Collapse>
      </Navbar>
    );
  }
}
