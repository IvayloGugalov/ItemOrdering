import React, { useState } from 'react';
import { Button, Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link, useNavigate } from 'react-router-dom';
import './NavMenu.css';
import useAuth from '../custom-hooks/useAuth';
import useLogOut from '../custom-hooks/useLogOut';

const NavMenu = () => {
  const [collapsed, setCollapsed] = useState(false);
  const { auth } = useAuth();
  const logOut = useLogOut();
  const navigate = useNavigate();

  const toggleNavbar = () => {
    setCollapsed(!collapsed);
  }

  const handleLogOut = async () => {
    await logOut();
    navigate('/home');
  }

  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
        <Container>
          <NavbarBrand tag={Link} to="/">Identity.Admin.React</NavbarBrand>
          <NavbarToggler onClick={toggleNavbar} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
            <ul className="navbar-nav flex-grow">
            <NavItem>
                <NavLink tag={Link} className="text-dark" to="/home">Home</NavLink>
              </NavItem>
              <NavItem>
                {!auth?.email && <NavLink NavLink tag={Link} className="text-dark" to="/login">Login</NavLink>}
              </NavItem>
              <NavItem>
                {!auth?.email && <NavLink tag={Link} className="text-dark" to="/register">Register</NavLink>}
              </NavItem>
              <NavItem>
                {auth?.roles?.find(role => ["翿"].includes(role)) && <NavLink tag={Link} className="text-dark" to="/admin-panel">Admin Panel</NavLink>}
              </NavItem>
              <NavItem>
                {auth?.email && <NavLink tag={Button} onClick={handleLogOut} style={{'background-color': 'white', border: 'none'}} >Log out</NavLink>}
              </NavItem>
            </ul>
          </Collapse>
        </Container>
      </Navbar>
    </header>
  );
}

export default NavMenu;