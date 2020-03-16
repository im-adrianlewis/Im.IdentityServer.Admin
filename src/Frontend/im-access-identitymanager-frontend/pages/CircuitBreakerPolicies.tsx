import * as React from 'react';
import { connect, DispatchProp } from 'react-redux';
import {Layout} from '../src/components/Layout';
import { IApplicationState } from 'src/stores';
import { CircuitBreakerPolicy } from '../src/stores/circuit-breaker-policies/types';
import { fetchRequest } from '../src/stores/circuit-breaker-policies/actions';

class CircuitBreakerPoliciesProps {
}

type CircuitBreakerPoliciesPropsWithDispatch = CircuitBreakerPoliciesProps & DispatchProp;

interface CircuitBreakerPoliciesState {
  loading: boolean;
  policies: CircuitBreakerPolicy[];
}

class CircuitBreakerPolicies extends React.Component<CircuitBreakerPoliciesPropsWithDispatch, CircuitBreakerPoliciesState> {
  constructor(props: Readonly<CircuitBreakerPoliciesPropsWithDispatch>) {
    super(props);
    
    this.state = {
      loading: false,
      policies: []
    };
  }

  public render () {
    return (
      <Layout showNavMenu={true}>
        <div>
          <button onClick={() => {
            return this.dispatchFetchRequest();
          }}>Fetch</button>
        </div>
        <div>
          {this.state.policies.map(policy => (
            <div>
              {policy.service}
            </div>
          ))}
        </div>
      </Layout>
    );
  }

  private dispatchFetchRequest() {
    this.props.dispatch(fetchRequest());
  }
}

function mapStateToProps(state: IApplicationState) {
  return {
    loading: state.circuitBreakerPolicies.loading,
    policies: state.circuitBreakerPolicies.data
  };
}

export default connect(mapStateToProps)(CircuitBreakerPolicies);
