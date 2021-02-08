import React, { useEffect, useState } from "react";
import {
    Button,
    Modal,
    ModalHeader,
    ModalBody,
    ModalFooter,
    Form,
    FormGroup,
    Label,
    Input,
  } from "reactstrap";

export const ViewDeveloperModal = ({ developer, isModalOpen, toggleModal }) => {

  const [firstName, setFirstName] = useState()
  const [lastName, setLastName] = useState()
  const [age, setAge] = useState()
  
  useEffect(()=>{
      console.log(developer)
      setFirstName(developer.firstName)
      setLastName(developer.lastName)
      setAge(developer.age)
  }, [developer])

  return (
    <div>
      <Modal isOpen={isModalOpen} toggle={toggleModal}>
        <ModalHeader toggle={toggleModal}>Add Developer Page</ModalHeader>
        <Form>
          <ModalBody>
            <FormGroup className="mb-4">
              <Label for="firstName">First Name</Label>
              <Input
                id="firstName"
                name="firstName"
                placeholder="First Name"
                value={firstName}       
                disabled={true}         
              />
            </FormGroup>
            <FormGroup className="mb-4">
              <Label for="lastName">Last Name</Label>
              <Input
                id="lastName"
                name="lastName"
                placeholder="Last Name"
                value={lastName}
                disabled={true}
              />
            </FormGroup>
            <FormGroup>
              <Label for="age">Age</Label>
              <Input
                type="number"
                id="age"
                name="age"
                placeholder="Age"
                value={age}
                disabled={true}
              />
            </FormGroup>
          </ModalBody>
        </Form>
      </Modal>
    </div>
  );
};
