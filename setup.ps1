$bucketname="documentstorage"
$compartmentId="ocid1.compartment.oc1..aaaaaaaap5x37wali6koqc2kzdqgswmkydzgkl2gfmseyile7iqqejj7hwtq"
$namespace="ax2w5fadcgcx"

$Env:OCI_CLI_SUPPRESS_FILE_PERMISSIONS_WARNING="True"

oci os bucket delete --name $bucketname --empty --force
oci os bucket create --compartment-id $compartmentId --name $bucketname --namespace-name $namespace

oci os object put -bn $bucketname --name tuono-owners-manual.pdf --file .\upload\tuono-owners-manual.pdf --namespace $namespace

#oci os object list --bucket-name $bucketname

#create knowledge base
$knowledgebaseId=oci generative-ai-agent knowledge-base create-default-kb --compartment-id $compartmentId

#create datasource
oci generative-ai-agent data-source create-object-storage-ds --compartment-id $compartmentId --knowledge-base-id $knowledgebaseId

#create agent
#oci generative-ai-agent agent create --compartment-id $compartmentId --description "demo agent" --display-name "sourcecodeagent" --welcome-message "Hello, ask me about the source code!"

#list datasources
#oci generative-ai-agent data-source list --all --compartment-id $compartmentId