export type RouteType = {
    path: string,
    protected: boolean,
    element: React.FC
}

export type ProtectedRouteProps = {
  isProtected: boolean
}
