import Router from 'next/router';

export type RouteCompleteHandler = (url: string) => void;

const onRouteCompleteEvents = new Array<RouteCompleteHandler>();

Router.events.on('routeChangeComplete', (url: string) => {
  onRouteCompleteEvents.forEach(fx => fx(url));
});

export function addRouteCompleteEvent(fx: RouteCompleteHandler) {
  onRouteCompleteEvents.push(fx);
}  
