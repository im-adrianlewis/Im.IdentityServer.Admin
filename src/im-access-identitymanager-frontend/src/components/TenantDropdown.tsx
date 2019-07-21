import * as react from 'react';
import { Dropdown, DropdownItem, DropdownToggle, DropdownMenu } from 'reactstrap';
import tenants from '../constants/tenants';
import css from '../../css/index.scss';

interface TenantDropdownProps {
  defaultTenant?: string;
  availableTenants?: string[];

  onSelectedTenant(tenant: string): void;
}

interface TenantDropdownState {
  selectedTenant: string | null;
  dropdownOpen: boolean;
}

class TenantDropdown extends react.Component<TenantDropdownProps, TenantDropdownState> {
  constructor(props: Readonly<TenantDropdownProps>) {
    super(props);

    this.state = {
      selectedTenant: props.defaultTenant ? props.defaultTenant : null,
      dropdownOpen: false
    };
  }

  public render() {
    var boundSelectedTenant = this.onSelectedTenant.bind(this);

    var items = tenants
      .filter(tenant => tenant !== 'Immediate')
      .map(tenant =>
      (
        <DropdownItem cssModule={css}
            key={tenant}
            active={this.state.selectedTenant === tenant}
            onClick={() => boundSelectedTenant(tenant)}>
          {tenant}
        </DropdownItem>
      ));

    return (
      <Dropdown cssModule={css} isOpen={this.state.dropdownOpen} toggle={this.toggle.bind(this)} setActiveFromChild>
        <DropdownToggle cssModule={css} caret>
          Brand
        </DropdownToggle>
        <DropdownMenu cssModule={css}>
          <DropdownItem cssModule={css} header>Brand</DropdownItem>
          <DropdownItem cssModule={css} 
              key="Immediate"
              active={this.state.selectedTenant === 'Immediate'}
              onClick={() => boundSelectedTenant('Immediate')}>
            Immediate
          </DropdownItem>
          <DropdownItem cssModule={css} divider />
          {items}
        </DropdownMenu>
      </Dropdown>
    );
  }

  private toggle(): void {
    this.setState((prevState: TenantDropdownState) => ({
      dropdownOpen: !prevState.dropdownOpen,
      selectedTenant: prevState.selectedTenant
    }));
  }

  private onSelectedTenant(tenant: string): void {
    this.props.onSelectedTenant(tenant);
  }
}

export default TenantDropdown;
