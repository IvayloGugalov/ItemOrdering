import React from 'react';
import { Routes, Route } from 'react-router-dom'
import Layout from './components/Layout';
import CreateUser from './components/admin_panel/CreateUser';
import Login from './components/login_register_forms/Login';
import Register from './components/login_register_forms/Register';
import CreateRole from './components/admin_panel/CreateRole';
import AdminPanel from './components/admin_panel/AdminPanel';
import UsersTable from './components/admin_panel/UsersTable';
import RequiresAuth from './components/RequiresAuth';
import PersistLogin from './components/login_register_forms/PersistLogin';
import Missing from './components/Missing';
import Unauthorized from './components/Unauthorized';
import Home from './components/Home';

import './custom.css';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  return (
    <Routes>
      <Route path='/' element={<Layout />} >
        <Route path='/login' element={<Login/>} />
        <Route path='/register' element={<Register/>} />
        <Route path='unauthorized' element={<Unauthorized/>} />
        <Route path='/home' element={<Home/>}/>

        {/* protected routes */}
        {/* Pass the role inside the array object */}
        {/* API calls to check for new roles?? */}
        <Route element={<PersistLogin />}>
          <Route element={<RequiresAuth allowedRoles={["翿"]} />}>
            <Route path='/admin-panel' element={<AdminPanel />} />
            <Route path='/add-user' element={<CreateUser/>} />
            <Route path='/add-role' element={<CreateRole/>} />
            <Route path='/get-users' element={<UsersTable/>} />
          </Route>
        </Route>

        {/* default page */}
        <Route path='*' element={<Missing/>} />
      </Route>

    </Routes>
  );
}

export default App;
