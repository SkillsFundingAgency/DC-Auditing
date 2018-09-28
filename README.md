# DC-Auditing

Auditing components for adding auditing. A component exists for auditing events from software, and a component for persisting audit messages to the SQL database. Auditing use an Azure Service Bus queue between the software wanting to audit and the peristing service for reliability.

## Auditing

Add a reference to ESFA.DC.Auditing.Interface in projects that wish to audit. Use the implementation project for a concrete instance of the auditor at runtime.

The concrete implementation requires a configured IQueuePublishService for persisting queue message to the queue.

## Persisting

The ESFA.DC.Auditing.Persistence project provides an implementation of the persisting logic. This component should be hosted in a service.