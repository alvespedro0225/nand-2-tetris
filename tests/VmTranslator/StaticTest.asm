@111

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@333

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@888

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@TEMP.8

// pops the top of the stack and stores at equivalent memory register
D=A
@R13
M=D
@SP
M=M-1
A=M
D=M
@R13
A=M
M=D
@TEMP.3

// pops the top of the stack and stores at equivalent memory register
D=A
@R13
M=D
@SP
M=M-1
A=M
D=M
@R13
A=M
M=D
@TEMP.1

// pops the top of the stack and stores at equivalent memory register
D=A
@R13
M=D
@SP
M=M-1
A=M
D=M
@R13
A=M
M=D
@TEMP.3

// pushes to sp
D=M
@SP
A=M
M=D
@SP
M=M+1
@TEMP.1

// pushes to sp
D=M
@SP
A=M
M=D
@SP
M=M+1
//adds top stack number to D and goes to the adress of the second
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M    
       
// subtracts sp[n-2] and sp[n-1]
M=M-D
// incremets sp
@SP
M=M+1

@TEMP.8

// pushes to sp
D=M
@SP
A=M
M=D
@SP
M=M+1
//adds top stack number to D and goes to the adress of the second
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M    
       
// adds sp[n-2] and sp[n-1]
M=D+M
// incremets sp
@SP
M=M+1

