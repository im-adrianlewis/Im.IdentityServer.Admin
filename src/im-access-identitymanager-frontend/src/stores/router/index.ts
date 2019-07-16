import Router from 'next/router';

export type RouteCompleteHandler = (url: string) => void;

export function addRouteCompleteEvent(fx: RouteCompleteHandler) {
  onRouteCompleteEvents.push(fx);
}

const onRouteCompleteEvents = new Array<RouteCompleteHandler>();

Router.events.on('routeChangeComplete', (url) => {
  onRouteCompleteEvents.forEach(fx => fx(url));
});
