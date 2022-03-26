import React from 'react';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { Container } from 'react-bootstrap';
import OptionCard from './OptionCard';
import { Link } from 'react-router-dom';

const AdminPanel = () =>{
  return (
    <Container>
      <h1>Admin panel</h1>

      <Row style={{marginTop:'1rem'}}>
        <Col md={{ span: 4 }}>
          <Link to="/add-user" style={{ textDecoration: 'none'}}>
            <OptionCard
              title='Add users'
              description='Creating new users.'
              image='' />
          </Link>
        </Col>
      </Row>

      <Row style={{marginTop:'1rem'}}>
        <Col md={{ span: 4 }}>
          <Link to="/get-users" style={{ textDecoration: 'none'}}>
            <OptionCard
              title='See all users'
              description='View all registered users.'
              image='' />
          </Link>
        </Col>
      </Row>

      <Row style={{marginTop:'1rem'}}>
        <Col md={{ span: 4 }}>
          <Link to="/add-role" style={{ textDecoration: 'none'}}>
            <OptionCard
              title='Create a new role'
              description='Creating a new role for your users.'
              image='' />
          </Link>
        </Col>
      </Row>
    </Container>
  );
}

export default AdminPanel;