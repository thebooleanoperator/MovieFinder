import { BrowserRouter, Route, Routes } from "react-router-dom";
import { getRoutes } from "./routes";

export const PrestoRouter = () => {
    const prestoRoutes = getRoutes()

    return (
        <BrowserRouter>
            <Routes>
                {prestoRoutes.map((route) => (
                    <Route path={route.path} element={<route.element/>}></Route>
                ))}
            </Routes>
        </BrowserRouter>
    )
}