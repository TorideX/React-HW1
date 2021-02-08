import React, { useEffect } from "react";
import tt, { Draggable } from "react-beautiful-dnd";

export const DraggableTableColumn = ({ columnName, index, developers }) => {
   
    useEffect(()=> {

    }, [developers])

  return (
    <div style={{marginRight:'50px'}}>
      <Draggable draggableId={columnName} index={index} key={index}>
        {(provided, snapshot) => (
          <div  {...provided.draggableProps} ref={provided.innerRef} >
            <h1 style={{backgroundColor:snapshot.isDragging?'green':'white'}} {...provided.dragHandleProps}>{columnName}</h1>
            {developers.map((devs,i) => (
                <h4 key={i}>{devs[columnName]}</h4>
            ))}
          </div>
        )}
      </Draggable>
    </div>
  );
};
