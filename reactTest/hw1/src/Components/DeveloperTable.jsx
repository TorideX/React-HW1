import React, { useState } from "react";
import { Button, Table } from "reactstrap"
import { ViewDeveloperModal } from "./ViewDeveloperModal";

export const DeveloperTable = ({ developers }) => {


  const [selectedDeveloper, setSelectedDeveloper] = useState({})
  const [viewModalIsOpen, setViewModalIsOpen] = useState(false)

  const viewModalToggle = () =>{
      setViewModalIsOpen(vm=>vm=!vm)
  }

  const viewButtonClick = (index) =>{
      setSelectedDeveloper(developers[index])
      console.log(index)
      console.log(developers[index])
      viewModalToggle()
  }

  return (
    <div>        
      <ViewDeveloperModal developer={selectedDeveloper} isModalOpen={viewModalIsOpen} toggleModal={viewModalToggle} />
      <Table>
        <thead>
          <tr>
            <th>#</th>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Age</th>
            <th>Buttons</th>
          </tr>
        </thead>
        <tbody>
          {
              developers.map((d,i)=>(
                <tr key={i}>
                    <th scope="row">{i + 1}</th>
                    <td>{d.firstName}</td>
                    <td>{d.lastName}</td>
                    <td>{d.age}</td>
                    <td>
                        <Button color="outline-primary" style={{marginRight:20}} onClick={()=>viewButtonClick(i)}>View</Button>
                        <Button color="outline-success">Edit</Button>
                    </td>
                </tr>
              ))
          }
        </tbody>
      </Table>
    </div>
  );
};
