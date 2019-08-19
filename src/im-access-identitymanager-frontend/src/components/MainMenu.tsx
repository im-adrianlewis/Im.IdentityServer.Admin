import * as React from 'react';
import { Navbar, NavbarBrand, NavbarToggler, Collapse } from 'reactstrap';
import css from '../../css/index.scss';
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
      <Navbar color="light" light expand="md" cssModule={css}>
        <NavbarToggler id="navbar-toggler" onClick={this.toggle} cssModule={css} />
        <NavbarBrand href="/" cssModule={css}>Identity Manager</NavbarBrand>
        <Collapse isOpen={this.state.isOpen} navbar cssModule={css}>
          <MainNav />
          <AccountNav />
        </Collapse>
      </Navbar>
    );
  }
}
