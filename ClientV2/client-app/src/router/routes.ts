import { Login } from "../Components/Login/Login"
import { Register } from "../Components/Register/Register"

export type RouteType = {
    path: string,
    protected: boolean,
    element: () => JSX.Element
}

const ROUTES: RouteType[] = [
    {
        path: '/login',
        protected: false,
        element: Login
    },
    {
      path: '/register',
      protected: false,
      element: Register
    }
]

export const getRoutes = () => {
    return ROUTES
}