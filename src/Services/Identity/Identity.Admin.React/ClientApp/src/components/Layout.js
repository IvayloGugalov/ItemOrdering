import React from 'react';
import { Outlet } from 'react-router-dom';
import Container from 'reactstrap/lib/Container';
import NavMenu from './NavMenu';

const Layout = () => {
  return (
    <main className='App'>
      <NavMenu />
      <Container>
        <Outlet />
      </Container>
    </main>
  )
}

export default Layout;