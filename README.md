# ScoringAPI
--Come up with a scoring algorithm that given some parameters it can give a customer a loan limit.

{
    "DataSet1": {
        "DateOfBirth": "2000-20-07",
        "Email": "johndoe@gmail.com",
        "IdNumber": "12345678",
        "Name": "John Doe",
        "Phone": "254711000001"
    },
    "DataSet2": {
        "DateOfBirth": "2000-20-07",
        "IdNumber": "12345678",
        "Name": "Doe John",
        "Phone": [
            "254711000001",
            "254711000002"
        ]
    },
    "DataSet3": {
        "Grade": "BB",
        "Probability": "3.8"
    }
}

-Dataset1 contains data submitted by the customer via form.
-Dataset2 and Dataset3 will be used to validate Dataset1.

The overall score a customer can get is 100% and the score is distributed as follows.
1. KYC weighted score - 40%
2. Grade - 60%

To compute the KYC score, follow the rules below
1. Id numbers must match
2. The customer must be over 22 years of age
3. The names should match at least 70%
4. At least one mobile number must match.

To compute the Grade output weight, follow the rules below
1. Each Grade has its own weight as shown below
Score Probability	Weights
AA			1
BB			0.9
CC			0.8
DD			0.7
EE			0.6
FF			0.6
GG			0.5
HH			0.4

2. Given the age of the customer, use the grading tiers below to determine the outcome
<?xml version="1.0" encoding="utf-8"?>
<Tier xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ScoreOutputGrades>
        <Score LowerAgeLimit="22" UpperAgeLimit="24" GradesOutPut="AA,BB" />
        <Score LowerAgeLimit="25" UpperAgeLimit="60" GradesOutPut="AA,BB,CC,DD,EE,FF,GG" />
    </ScoreOutputGrades>
</Tier>
The above tiers represent the considerable grades per age group.

Below is the expected output:
	Provide an API that can receive the above payload
	After receiving it, perform the above operations and provide response after processing
	Provide an API to query for the result after the above is completed.

