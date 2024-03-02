import { BrowserRouter, Route, Routes } from "react-router-dom";
import { ProtectedRouteProps, RouteType } from "../types/routeType";
import { MainLayout } from "../layouts/MainLayout";
import { LoginPage } from "../pages/Login/LoginPage";
import { RegisterPage } from "../pages/Register/RegisterPage";

const PrestoRoutes: RouteType[] = [
  {
      path: '/login',
      protected: false,
      element: LoginPage
  },
  {
    path: '/register',
    protected: false,
    element: RegisterPage
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