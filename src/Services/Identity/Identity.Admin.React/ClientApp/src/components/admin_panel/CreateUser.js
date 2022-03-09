import React, { useState, useEffect } from 'react'
import Form from 'react-bootstrap/Form'
import FloatingLabel from 'react-bootstrap/FloatingLabel'
import Button from 'react-bootstrap/Button'
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { variables } from '../../Variables';
import InputForm from './InputForm';

function CreateUser() {

  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');
  const [roles, setRoles] = useState([0]);
  const [selectedRole, setSelectedRole] = useState(roles[0]);
  const [error, setError] = useState('');

  
  useEffect(() =>{
      async function fetchRoles() {
      try{
        let response = await fetch(variables.API_URL + "admin/get-roles");
        setRoles(await response.json());
      }
      catch (e){
        console.error(e);
      }
    };
    fetchRoles();
  }, []);

  // https://www.freecodecamp.org/news/how-to-work-with-multiple-checkboxes-in-react/
  // const [selectedRoles, setSelectedRoles] = useState([]);
  // const [checkedState, setCheckedState] = useState(new Array(roles.length).fill(false));
  
  // const handleOnChange = (selectedRole, position) => {
  //   const updatedCheckedState = checkedState.map((item, index) =>
  //     index === position ? !item : item
  //   );

  //   // console.log(updatedCheckedState)
  //   setCheckedState(updatedCheckedState);

  //   console.log(roles.find(x => x == selectedRole));
  //   console.log(selectedRoles.find(x => x == selectedRole));

  //   updatedCheckedState.reduce(
  //     (previous, currentState, index) => {
  //       if (currentState == true) {
  //         console.log('adding')
  //         setSelectedRoles((prevSelectedRoles) => {
  //           return [...prevSelectedRoles, roles[index]];
  //         })
  //       }
  //       else if(selectedRoles.find(x => x == roles[index])){
  //         console.log('removing')
  //         setSelectedRoles((selectedRoles) => {
  //           return selectedRoles.filter((role) => role !== selectedRole);
  //       })}
  //     }
  //   );

  //   console.log(selectedRoles)
  // };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (firstName && lastName && email && userName && password) {
      
      fetch(variables.API_URL + 'admin/create-user', {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          firstName: firstName,
          lastName: lastName,
          email: email,
          userName: userName,
          password: password,
          roles: [selectedRole]})
      })
      .then(async response =>{
        if (response.ok){
          alert("Success")

          setFirstName('');
          setLastName('');
          setUserName('');
          setEmail('');
          setPassword('');
        }
        else{
          throw new Error('Network response was not ok.');
        }
      },(error)=>{
        alert('Failed');
      });    
    }
    else {
      setTimeout(() => {
        console.log({firstName, lastName, email, password, userName})
        setError('Fill all the text boxes')
      });
      setTimeout(() => {
        setError('')
      },3000);
    }
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

          {/* {roles.map((role, index) => {
            return (
              <div key={role} className="mb-3">
                <Form.Check
                  inline
                  id={index}
                  label={role}
                  name={role}
                  checked={checkedState[index]}
                  onChange={() => handleOnChange(role, index)} />
              </div>
            );
          })} */}

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