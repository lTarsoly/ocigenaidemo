This repository contains a demonstration project of OCI Generative AI Agents and OCI .NET SDK.

1. install OCI CLI: https://docs.oracle.com/en-us/iaas/Content/API/SDKDocs/climanualinst.htm
2. replace compartment-, tenant-, and endpoint IDs (and names) in both the attached PS script and in the Program.cs file as well (with the resource IDs/names you have created)
3. create OCI Generative AI Knowledge Base and Agent either via the attached PS script, or maually via the OCI Console
4. build the .NET 9 C# solution under client folder
5. log in to OCI via `oci session authenticate --profile-name DEFAULT`
6. run the compiled console application and supply a question about the document uploaded to the object storage bucket
