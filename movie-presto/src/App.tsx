import React from 'react';
import logo from './logo.svg';
import { Outlet, Route, Routes, RedirectFunction as Redirect, Navigate, RouteProps } from 'react-router-dom'

import './App.css';

const routes = [
  {
    path: '/',
    element: <div>Login</div>,
    protected: false 
  },
  {
    path: '/Dashboard',
    element: <div>Dash</div>,
    protected: true 
  },
  {
    path: '/Favortes',
    element: <div>Favorites</div>,
    protected: true 
  }
]

const ProtectedRoute: React.FC= () => {
  if (true) {
    return <Navigate to='/' />;
  }
  return <Outlet />;
};

const App: React.FC = () => {
  return ( 
    <Routes>
      {routes.map((route, idx) => (
        route.protected
          ? <Route key={'protected'} element={<ProtectedRoute />} > 
              <Route key={idx} {...route}></Route>
            </Route>
          : <Route key={idx} {...route} />
      ))}
    </Routes>
  );
}

export default App;
