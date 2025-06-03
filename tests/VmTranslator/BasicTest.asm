@10

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
// adds default segment
@0
D=A
@LCL
A=D+M

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
@21

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@22

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
// adds default segment
@2
D=A
@ARG
A=D+M

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
// adds default segment
@1
D=A
@ARG
A=D+M

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
@36

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
// adds default segment
@6
D=A
@THIS
A=D+M

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
@42

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@45

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
// adds default segment
@5
D=A
@THAT
A=D+M

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
// adds default segment
@2
D=A
@THAT
A=D+M

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
@510

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@11
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
// adds default segment
@0
D=A
@LCL
A=D+M

// pushes to sp
D=M
@SP
A=M
M=D
@SP
M=M+1
// adds default segment
@5
D=A
@THAT
A=D+M

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

// adds default segment
@1
D=A
@ARG
A=D+M

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

// adds default segment
@6
D=A
@THIS
A=D+M

// pushes to sp
D=M
@SP
A=M
M=D
@SP
M=M+1
// adds default segment
@6
D=A
@THIS
A=D+M

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

@11
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

    
       
// adds sp[n-2] and sp[n-1]
M=D+M
// incremets sp
@SP
M=M+1

