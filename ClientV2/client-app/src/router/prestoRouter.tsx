import { BrowserRouter, Route, Routes } from "react-router-dom";
import { ProtectedRouteProps, RouteType } from "../types/routeType";
import { MainLayout } from "../layouts/MainLayout";
import { Login } from "../Components/Login/Login";
import { Register } from "../Components/Register/Register";

const PrestoRoutes: RouteType[] = [
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


const ProtectedRoute = ({isProtected}: ProtectedRouteProps) => {
  // TODO: validate auth
  if (isProtected) {
    return <MainLayout />
  }
  
  return <MainLayout />
}

export const PrestoRouter = () => {
    return (
      <BrowserRouter>
          <Routes>
              {PrestoRoutes.map((route) => (
                  <Route key={route.path} element={<ProtectedRoute isProtected={route.protected} />}>
                    <Route path={route.path} element={<route.element />} />
                  </Route>
              ))}
          </Routes>
      </BrowserRouter>
    )
}