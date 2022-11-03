import React from 'react';

import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from 'react-router-dom';

// import { Provider } from './context/Provider';

import Login from './pages/Login';
// import Register from './pages/Register';
// import Feed from './pages/Feed';

export default function App() {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route exact path="/" element={<Navigate replace to="/login" />} />
        {/* <Route path="/feed" element={<Feed />} /> */}
        {/* <Route path="/register" element={<Register />} /> */}
      </Routes>
    </Router>
  );
}
