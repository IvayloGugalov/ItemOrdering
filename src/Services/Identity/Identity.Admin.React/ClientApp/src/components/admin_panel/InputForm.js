import React, { useMemo } from 'react'
import Form from 'react-bootstrap/Form'

function InputForm({property, placeHolder, type, handleChange}) {
  const currentType = useMemo(() => type);

  return (
    <Form.Floating>
      <Form.Control
          type={currentType}
          name={property.name}
          value={property.value}
          onChange={(e) => handleChange(e.target.value)}
          placeholder={placeHolder} />
      <Form.Text className="text-muted" />
    </Form.Floating>
  );
}

export default InputForm;