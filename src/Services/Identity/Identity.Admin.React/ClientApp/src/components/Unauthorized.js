import React from 'react';
import { useNavigate } from "react-router-dom"
import Button from 'react-bootstrap/Button'

const Unauthorized = () => {
  const navigate = useNavigate();

  const goBack = () => navigate(-1);

  return (
    <section>
      <h1>Unauthorized</h1>
      <br />
      <p>You do not have access to the requested page.</p>
      <div className="flexGrow">
        <Button onClick={goBack} variant="danger" >Go Back</Button>
      </div>
    </section>
  )
}

export default Unauthorized