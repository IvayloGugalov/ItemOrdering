import React, { useState, useEffect } from 'react'
import Form from 'react-bootstrap/Form'
import FloatingLabel from 'react-bootstrap/FloatingLabel'
import Button from 'react-bootstrap/Button'
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import InputForm from './InputForm';
import { variables } from '../../Variables';
import useAxiosPrivate from '../../custom-hooks/useAxiosPrivate'

const CreateUser = () => {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');
  const [roles, setRoles] = useState([0]);
  const [selectedRole, setSelectedRole] = useState(roles[0]);
  const [error, setError] = useState('');

  const axiosPrivate = useAxiosPrivate();

  const getRoles = async () => {
    try {
      const response = await axiosPrivate.get(variables.IDENTITY_API_URL + "admin/get-roles",
      {
        headers: { 'Content-Type': 'application/json' },
        withCredentials: true
      });

      const roles = response?.data;
      setRoles(roles);
    }
    catch (e){
      console.error(e);
    }
  }

  useEffect(() => {
    setError('');
  }, [firstName, lastName, email, userName, password, roles]);

  useEffect(() => {
    getRoles();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (firstName && lastName && email && userName && password) {
      try {
        const response = await axiosPrivate.post(variables.IDENTITY_API_URL + 'admin/create-user',
        JSON.stringify({
          firstName: firstName,
          lastName: lastName,
          email: email,
          userName: userName,
          password: password,
          roles: [selectedRole]}),
          {
            headers: { 'Accept': 'application/json' },
            withCredentials: true
          }
        );

        if (response?.status === 200){
          alert("Success")

          setFirstName('');
          setLastName('');
          setUserName('');
          setEmail('');
          setPassword('');
        }
      } catch (err) {
        alert(`Failed: ${err}`);
        setTimeout(() => {
          setError('')
          }, 3000);
      }
    }
    else {
      setTimeout(() => {
        setError('Fill all the text boxes');
      });
    };
  };

  return (
  <article className='justify-content-md-center' style={{ marginTop: '4rem'}} >

    <h2 style={{textAlign: 'center'}} >Creating a new user</h2>

    <Form onSubmit={handleSubmit} className='mb-6' >

      <Row className="mb-6">
        <Col>
          <InputForm property={firstName} placeHolder="Enter First Name" type="text" handleChange={setFirstName} />
        </Col>

        <Col>
          <InputForm property={lastName} placeHolder="Enter Last Name" type="text" handleChange={setLastName}/>
        </Col>
      </Row>

      <Row className="mb-6">
        <Col>
          <InputForm property={userName} placeHolder="Enter User Name" type="text" handleChange={setUserName} />
        </Col>

        <Col>
          <InputForm property={email} placeHolder="Enter email" type="email" handleChange={setEmail} />
        </Col>
      </Row>

      <Row className="mb-6">
        <Col md={6}>
          <InputForm property={password} placeHolder="User Password" type='password' handleChange={setPassword} />
        </Col>

        <Col md={6}>
          <FloatingLabel controlId="floatingSelectGrid">
            <Form.Label style={{marginRight:'1rem'}}>Select role</Form.Label>
            <Form.Select
              aria-label="Role selection"
              onChange={(e) => setSelectedRole(e.target.value)}
              >
              {roles.map((role) => {
                  return <option value={role} key={role}>{role}</option>
                })}
            </Form.Select>
          </FloatingLabel>

        </Col>
      </Row>

      <Row style={{textAlign: 'center'}}>
        <Col md={{ span: 6, offset: 3 }} style={{justifyContent: 'center'}}>
          <Button variant="primary" type="submit">
            Submit
          </Button>
        </Col>
      </Row>

      <h2 style={{textAlign: 'center'}}>{error}</h2>

    </Form>
  </article>);
}

export default CreateUser;