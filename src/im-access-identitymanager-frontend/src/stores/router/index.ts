import Router from 'next/router';

export function addRouteCompleteEvent(fx: (url: string) => void) {
  onRouteCompleteEvents.push(fx);
}

const onRouteCompleteEvents = [];

Router.events.on('routeChangeComplete', (url) => {
  onRouteCompleteEvents.forEach(fx => fx(url));
});
